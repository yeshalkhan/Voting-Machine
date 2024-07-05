using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class Voter
    {
        string voterName;
        string cnic;
        string selectedPartyName;
        public Voter()
        {

        }
        public Voter(string VoterName, string cnic, string selectedPartyName)
        {
            this.voterName = VoterName;
            this.cnic = cnic;
            this.selectedPartyName = selectedPartyName;
        }
        public string SelectedPartyName
        {
            set { selectedPartyName = value; }
            get { return selectedPartyName; }
        }
        public bool hasVoted()
        {
            if (selectedPartyName == null)
                return false;
            return true;
        }
        public string Cnic
        {
            get { return cnic; }
            set { cnic = value; }
        }
        public string VoterName
        {
            get { return voterName; }
            set { voterName = value; }
        }
    }
}
