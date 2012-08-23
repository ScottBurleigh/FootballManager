using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CollegeFootballManager
{
    [TestFixture]
    public class TestFixtureName
    {
        [Test]
        public void ProspectGenerator_should_generate_prospects()
        {
            var numberOfProspectsToGenerate = 100;
            var seasonId = 1;
            
            ProspectGenerator prospectGenerator = new ProspectGenerator(numberOfProspectsToGenerate, seasonId);

            prospectGenerator.Generate();

            var prospects = FootballManagerDatabase.GetAllProspectsBySeason(seasonId);

            Assert.AreEqual(prospects.Count(), numberOfProspectsToGenerate);

        }

    }

    public class FootballManagerDatabase
    {
        private static HashSet<Prospect> _Prospects = new HashSet<Prospect>();

        public static IEnumerable<Prospect> GetAllProspectsBySeason(int seasonId)
        {
            return _Prospects.Where(x => x.IsInSeason(seasonId)).ToList();
        }

        public static void AddProspect(Prospect prospect)
        {
            _Prospects.Add(prospect);
        }
    }

    public class ProspectGenerator
    {
        readonly int _numberOfProspectsToGenerate;
        readonly int _seasonId;

        public ProspectGenerator(int numberOfProspectsToGenerate, int seasonId)
        {
            _numberOfProspectsToGenerate = numberOfProspectsToGenerate;
            _seasonId = seasonId;
        }

        public void Generate()
        {
            for(int i = 0; i<_numberOfProspectsToGenerate; i++)
            {
                Prospect p = new Prospect("", "", _seasonId);
                FootballManagerDatabase.AddProspect(p);
            }
        }
    }

    public class Prospect
    {
        readonly string _name;
        readonly string _position;
        readonly int _seasonId;

        public int SeasonId
        {
            get { return _seasonId; }
        }

        public Prospect(string name, string position, int seasonId)
        {
            _name = name;
            _position = position;
            _seasonId = seasonId;
            
        }

        public bool IsInSeason(int seasonId)
        {
            return _seasonId == seasonId;
        }
    }
}