using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FanFou.SDK.Objects;
using FanFou.SDK.Objects.Search;

namespace FanFou.SDK.API
{
    public class SavedSearches : RequestBase
    {
        public SavedSearches(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<SavedSearch> SavedSearchesCreateCallBack { get; set; }
        /// <summary>
        /// 收藏搜索关键字
        /// </summary>
        /// <param name="callback"></param>
        public void SavedSearchesCreate(Action<SavedSearch> callback)
        {

            SavedSearchesCreateCallBack = callback;
            var parameters = new Parameters();

            GetData("http://api.fanfou.com/saved_searches/create.json", parameters, SavedSearchesCreateEnd);
        }

        private void SavedSearchesCreateEnd(string json)
        {
            if (SavedSearchesCreateCallBack != null)
            {

                var result = Util.JsonToObject<SavedSearch>(json);
                SavedSearchesCreateCallBack(result);
            }
        }

        public Action<SavedSearch> SavedSearchesDestroyCallBack { get; set; }
        /// <summary>
        /// 删除收藏的搜索关键字
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        public void SavedSearchesDestroy(Action<SavedSearch> callback, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            SavedSearchesDestroyCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            PostData("http://api.fanfou.com/saved_searches/destroy.json", parameters, SavedSearchesDestroyEnd);
        }

        private void SavedSearchesDestroyEnd(string json)
        {
            if (SavedSearchesDestroyCallBack != null)
            {
                var result = Util.JsonToObject<SavedSearch>(json);
                SavedSearchesDestroyCallBack(result);
            }
        }

        public Action<SavedSearch> SavedSearchesShowCallBack { get; set; }
        /// <summary>
        /// 返回搜索关键字的详细信息
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        public void SavedSearchesShow(Action<SavedSearch> callback, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            SavedSearchesShowCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            GetData("http://api.fanfou.com/saved_searches/show.json", parameters, SavedSearchesShowEnd);
        }

        private void SavedSearchesShowEnd(string json)
        {
            if (SavedSearchesShowCallBack != null)
            {
                var result = Util.JsonToObject<SavedSearch>(json);
                SavedSearchesShowCallBack(result);
            }
        }

        public Action<ICollection<SavedSearch>> SavedSearchesListCallBack { get; set; }

        /// <summary>
        /// 列出登录用户保存的搜索关键字
        /// </summary>
        /// <param name="callback"></param>
        public void SavedSearchesList(Action<ICollection<SavedSearch>> callback)
        {

            SavedSearchesListCallBack = callback;


            GetData("http://api.fanfou.com/saved_searches/list.json", null, SavedSearchesListEnd);
        }

        private void SavedSearchesListEnd(string json)
        {
            if (SavedSearchesListCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<SavedSearch>>(json);
                SavedSearchesListCallBack(result);
            }
        }


    }
}
