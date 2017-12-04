using System.Security.Principal;

namespace Lightstone.WebClient.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        private IIdentity _Identity;
        private string _Role;
        private string _DisplayName;
        private int _TeamId;
        private string _TeamName;

        public CustomPrincipal(IIdentity identity, string role, string displayName, int teamId, string teamName)
        {
            this.Identity = identity;
            this.Role = role;
            this.DisplayName = displayName;
            this.TeamId = teamId;
            this._TeamName = teamName;
        }

        public IIdentity Identity { get => _Identity; set => _Identity = value; }
        public string Role { get => _Role; set => _Role = value; }
        public string DisplayName { get => _DisplayName; set => _DisplayName = value; }
        public int TeamId { get => _TeamId; set => _TeamId = value; }
        public string TeamName { get => _TeamName; set => _TeamName = value; }

        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(_Role))
            {
                return false;
            }

            return _Role.Equals(role);
        }
    }
}