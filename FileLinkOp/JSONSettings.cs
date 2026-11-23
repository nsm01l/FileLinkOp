using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLinkOp
{
    internal class JSONSettings
    {
        /// <summary>
        /// ウィンドウの設定
        /// </summary>
        public WindowSettings window { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchCondition search { get; set; }

        /// <summary>
        /// 自分自身のファイルの挙動
        /// </summary>
        [JsonConverter(typeof(JSONMeConverter))]
        public string me { get; set; }

        /// <summary>
        /// リンク対象ファイル
        /// </summary>
        [JsonConverter(typeof(JSONLinkListConverter))]
        public List<string> link { get; set; }
    }

    /// <summary>
    /// ウィンドウの設定
    /// </summary>
    internal class WindowSettings
    {
        /// <summary>
        /// ウィンドウタイトル
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// ウィンドウの色
        /// </summary>
        public string back { get; set; }

        /// <summary>
        /// 文字の色
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// ウィンドウの幅
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// ウィンドウの高さ
        /// </summary>
        public int height { get; set; }
    }

    /// <summary>
    /// 検索条件
    /// </summary>
    internal class SearchCondition
    {
        /// <summary>
        /// 拡張子を含めるか
        /// </summary>
        public bool contains_extension { get; set; }

        /// <summary>
        /// 条件の正規表現
        /// </summary>
        //[JsonConverter(typeof(JSONSearchConditionRegexConverter))]
        public string condition { get; set; }
    }
}
