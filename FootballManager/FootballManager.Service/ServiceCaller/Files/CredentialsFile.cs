using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCaller.Files
{
    [Serializable]
    class CredentialsModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    class CredentialsFile : FileBase<CredentialsModel>
    {
        protected override string FileName
        {
            get { return "credentials.dat"; }
        }
    }
}
