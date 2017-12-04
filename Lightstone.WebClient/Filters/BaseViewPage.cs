using Lightstone.WebClient.Authentication;
using System.Web.Mvc;

namespace Lightstone.WebClient.Filters
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }

        protected virtual bool? IsManager
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                return User.IsInRole("Manager");
            }
        }

        protected virtual string DisplayName
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                return User.DisplayName;
            }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }

        protected virtual bool? IsManager
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                return User.IsInRole("Manager");
            }
        }

        protected virtual string DisplayName
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                return User.DisplayName;
            }
        }

    }
}
