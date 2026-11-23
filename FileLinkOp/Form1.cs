using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileLinkOp
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        const string SETTING_FILE_NAME = "settings.json";

        /// <summary>
        /// 設定ファイル中身
        /// </summary>
        static JSONSettings SETTINGS;

        /// <summary>
        /// ファイルパスを取得する
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns></returns>
        delegate string GetFileNameDelegate(string filePath);

        /// <summary>
        /// ファイル名を取得する
        /// </summary>
        GetFileNameDelegate GetFileName;

        /// <summary>
        /// 対象ファイルか判定
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns></returns>
        delegate bool IsTargetFileDelegate(string filePath);

        /// <summary>
        /// 対象ファイルか判定
        /// </summary>
        private IsTargetFileDelegate IsTargetFile;

        /// <summary>
        /// ドラッグ&ドロップされた自分自身に対する処理
        /// </summary>
        /// <param name="filePath"></param>
        delegate void MyFileActionDelegate(string filePath);

        /// <summary>
        /// ドラッグ&ドロップされた自分自身に対する処理
        /// </summary>
        private MyFileActionDelegate myFileAction;

        /// <summary>
        /// リンク対象ファイルに対していろいろやる処理
        /// </summary>
        /// <param name="baseFilePath">drag drop file path</param>
        delegate void LinkedFileActionDelegate(string baseFilePath);

        /// <summary>
        /// リンク対象ファイルへの処理
        /// </summary>
        private List<LinkedFileActionDelegate> linkedFileActions;

        /// <summary>
        /// ベースパスを取得する処理
        /// </summary>
        /// <param name="targetFilePath">対象ファイルパス</param>
        /// <param name="folderPath">フォルダーパス</param>
        /// <returns></returns>
        delegate string GetBasePathDelegate(string targetFilePath, string folderPath);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 自分の場所
            var asm = Assembly.GetExecutingAssembly();
            var myPath = asm.Location;
            var myFolder = Path.GetDirectoryName(myPath);

            // binding
            this.DataBindings.Add(nameof(this.TopMost), chkOnTop, nameof(chkOnTop.Checked));

            // 設定ファイルを読み込む
            var settingFilePath = Path.Combine(myFolder, SETTING_FILE_NAME);
            var jsonText = File.ReadAllText(settingFilePath);
            SETTINGS = JsonConvert.DeserializeObject<JSONSettings>(jsonText);

            SetWindowDesign();
            
            CreateTargetFileChecker();

            CreateMyFileAction();

            linkedFileActions = CreateLinkedFileActions();
        }

        /// <summary>
        /// ウィンドウのデザインを設定する
        /// </summary>
        private void SetWindowDesign()
        {
            var win = SETTINGS.window;
            if (!string.IsNullOrEmpty(win.title))
            {
                this.Text = $"{win.title} - FileLinkOp";
            }

            if (Regex.IsMatch(win.back.ToLower(), "^#[0-9a-f]{6}$"))
            {
                this.BackColor = ColorTranslator.FromHtml(win.back);
            }
            if (Regex.IsMatch(win.text.ToLower(), "^#[0-9a-f]{6}$"))
            {
                lblDnD.ForeColor = ColorTranslator.FromHtml(win.text);
            }

            var width = (win.width < 158) ? 158 : win.width;
            var height = (win.height < 158) ? 158 : win.height;
            this.Size = new Size(width, height);
        }

        /// <summary>
        /// 対象ファイルか判定する処理を定義
        /// </summary>
        private void CreateTargetFileChecker()
        {
            var search = SETTINGS.search;
            if (search.contains_extension)
            {
                string gfp(string filePath)
                {
                    return Path.GetFileName(filePath);
                }
                GetFileName = gfp;
            }
            else
            {
                string gfp(string filePath)
                {
                    return Path.GetFileNameWithoutExtension(filePath);
                }
                GetFileName = gfp;
            }
            bool itf(string filePath)
            {
                var fileName = GetFileName(filePath);
                return Regex.IsMatch(fileName, search.condition);
            }
            IsTargetFile = itf;
        }

        /// <summary>
        /// 対象ファイル自身に対する操作を決める
        /// </summary>
        private void CreateMyFileAction()
        {
            var json = JObject.Parse(SETTINGS.me);
            switch (json.Property("action")?.Value.ToString())
            {
                case "delete":
                case "remove":
                    void rmv(string filePath)
                    {
                        FileSystem.DeleteFile(filePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    };
                    myFileAction = rmv;
                    break;

                case "open":
                case "show":
                    var exePath = json.Property("exe_path")?.Value.ToString();
                    if (string.IsNullOrEmpty(exePath))
                    {
                        void opn(string filePath)
                        {
                            var startProc = new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = filePath,
                                UseShellExecute = true,
                                CreateNoWindow = true,
                            };
                            System.Diagnostics.Process.Start(startProc);
                        }
                        myFileAction = opn;
                    }
                    else
                    {

                    }
                    break;

                case "move":
                    break;

                case "copy":
                    break;
            }
        }

        /// <summary>
        /// リンクファイルに対する処理を定義
        /// </summary>
        /// <returns></returns>
        private List<LinkedFileActionDelegate> CreateLinkedFileActions()
        {
            var ret = new List<LinkedFileActionDelegate>();

            var searchCondition = SETTINGS.search.condition;
            foreach (var link in SETTINGS.link)
            {
                var json = JObject.Parse(link);
                
                var targetFileReplace = json.Property("file")?.Value.ToString() ?? string.Empty;
                switch (json.Property("action")?.Value.ToString())
                {
                    case "delete":
                    case "remove":
                        var removeFolder = json.Property("folder")?.Value.ToString() ?? string.Empty;
                        var removeFolderFunc = GetBasePassFunc(removeFolder);

                        LinkedFileActionDelegate rmv = (filePath) =>
                        {
                            string basePath = removeFolderFunc(filePath, removeFolder);

                            var fp = GetFileName(filePath);
                            var targetFileName = Regex.Replace(fp, searchCondition, targetFileReplace);
                            var targetFilePath = Path.Combine(basePath, targetFileName);
                            if (File.Exists(targetFilePath))
                            {
                                FileSystem.DeleteFile(targetFilePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            }
                        };
                        ret.Add(rmv);
                        break;

                    case "move":
                        var destPathReplace = json.Property("path")?.Value.ToString() ?? string.Empty;
                        LinkedFileActionDelegate mov = (filePath) =>
                        {
                            var basePath = Path.GetDirectoryName(filePath);
                            var fp = GetFileName(filePath);
                            var targetFileName = Regex.Replace(fp, searchCondition, targetFileReplace);
                            var targetFilePath = Path.Combine(basePath, targetFileName);


                            var destFolderPath = Regex.Replace(fp, searchCondition, destPathReplace);
                            var destFilePath = Path.Combine(destFolderPath, targetFileName);

                            if (File.Exists(targetFilePath))
                            {
                                File.Move(targetFileName, destFilePath);
                            }
                        };
                        ret.Add(mov);
                        break;
                }
            }

            return ret;
        }

        /// <summary>
        /// basePath を計算するメソッドを返す
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        private static GetBasePathDelegate GetBasePassFunc(string folderPath)
        {
            if (Regex.IsMatch(folderPath.ToLower(), "^[a-z]:"))
            {
                // 絶対パス
                string ret(string t, string f)
                {
                    return f;
                }
                return ret;
            }
            else if (string.IsNullOrEmpty(folderPath)) 
            { 
                // 渡されたファイル基準
                string ret(string t, string f)
                {
                    return Path.GetDirectoryName(t);
                }
                return ret;
            }
            else
            {
                // 相対パス
                string ret(string t, string f)
                {
                    return Path.Combine(t, f);
                }
                return ret;
            }
        }

        /// <summary>
        /// ドラッグ&ドロップされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDnD_DragDrop(object sender, DragEventArgs e)
        {
            var dropFile = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            lblDnD.Enabled = false;

            try
            {
                foreach (var path in dropFile)
                {
                    if (IsTargetFile(path))
                    {
                        // DBG
                        //Console.WriteLine($"kita! {path}");

                        myFileAction(path);

                        foreach (var act in linkedFileActions)
                        {
                            act(path);
                        }
                    }
                }
            }
            finally
            {
                lblDnD.Enabled = true;
            }
        }

        /// <summary>
        /// ドラッグ&ドロップされようとするとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDnD_DragEnter(object sender, DragEventArgs e)
        {
            // ファイル以外は拒否
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
