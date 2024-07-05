using System;
using System.Collections.Generic;


namespace Assignment1
{

    class Candidate
    {
        // static int nextID = 1;
        int candidateID;
        string name;
        string party;
        int votes;

        int GenerateCandidateID()
        {
            Random random = new Random();
            int ID = ((int)DateTime.Now.Ticks + random.Next());
            return ID >= 0 ? ID : -1 * ID;
        }
        public Candidate()
        {

        }
        public Candidate(string name, string party)
        {
            candidateID = GenerateCandidateID();
            votes = 0;
            Name = name;
            Party = party;
        }
        public int CandidateID
        {
            set { candidateID = value; }
            get { return candidateID; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Party
        {
            get { return party; }
            set { party = value; }
        }
        public int Votes
        {
            set { votes = value; }
            get { return votes; }
        }
        public void IncrementVotes()
        {
            votes++;
        }
        public override string ToString()
        {
            return $" ID : {candidateID}, Name : {name}, Party : {party}, Votes : {votes}";
        }
    }
}
