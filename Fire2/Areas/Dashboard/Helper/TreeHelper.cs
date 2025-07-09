using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Fire2.Areas.Dashboard.Models;
using Fire2.Models;

namespace Fire2.Areas.Dashboard.Helper
{

    public class TreeHelper
    {
        private Model1 db = new Model1();

        public string GetTree()
        {
            //越來越多層寫不完
            //組字串
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            //第一層權限
            var parentPermission = db.Permissions.Where(x => x.ParentId == null).ToList();
            sb.Append(GetTreeString(parentPermission));
            sb.Append("]");
            return sb.ToString();
        }

        public string GetTreeString(ICollection<Permission> parentPermission)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var permission in parentPermission)
            {
                sb.Append("{");
                sb.Append($"\"id\": \"{permission.Code}\",");
                sb.Append($"\"text\": \"{permission.Subject}\"");
                if (permission.ChildPermissions.Count > 0)
                {
                    sb.Append($",\"children\": [");
                    sb.Append(GetTreeString(permission.ChildPermissions));
                    sb.Append("]");
                }
                sb.Append("},");
            }
            return sb.ToString();

        }
    }
}