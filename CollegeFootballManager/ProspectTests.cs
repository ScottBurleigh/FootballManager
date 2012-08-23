using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CollegeFootballManager
{
    [TestFixture]
    public class TestFixtureName
    {
        [SetUp]
        public void SetUp()
        {
            FootballManagerDatabase.Clear();
        }


        [Test]
        public void ProspectGenerator_should_generate_prospects()
        {
            var numberOfProspectsToGenerate = 100;
            var seasonId = 1;

            ProspectGenerator prospectGenerator = new ProspectGenerator(GetInMemoryFirstNameGenerator(), numberOfProspectsToGenerate, seasonId);

            prospectGenerator.Generate();

            var prospects = FootballManagerDatabase.GetAllProspectsBySeason(seasonId);

            Assert.AreEqual(prospects.Count(), numberOfProspectsToGenerate);

        }

        [Test]
        public void ProspectGenerator_should_generate_first_name_from_firstName_masterList()
        {
            var NameList = new List<string>() {"Andy", "Billy"};
            var nameGenerator = GetInMemoryFirstNameGenerator(NameList);
            ProspectGenerator prospectGenerator = new ProspectGenerator(nameGenerator, 1, 1);
            prospectGenerator.Generate();

            var prospect = FootballManagerDatabase.GetAllProspectsBySeason(1).ToList()[0];

            Assert.IsTrue(NameList.Contains(prospect.FirstName));

        }

        InMemoryNameGenerator GetInMemoryFirstNameGenerator(List<string> NameList = null)
        {
            
            return new InMemoryNameGenerator(NameList ?? new List<string>(){"Jack"});
            
        }
    }

    public interface INameGenerator
    {
        string Generate();
    }

    public class InMemoryNameGenerator : INameGenerator
    {
        readonly IEnumerable<string> _nameList;

        public InMemoryNameGenerator(IEnumerable<string> nameList)
        {
            _nameList = nameList;
        }

        public string Generate()
        {
            var RandNum = new System.Random();
            int MyRandomNumber = RandNum.Next(0, _nameList.ToList().Count);
            return _nameList.ToList()[MyRandomNumber];
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

        public static void Clear()
        {
            _Prospects.Clear();
        }
    }

    public interface IGenerator
    {
        void Generate();
    }

    public class ProspectGenerator : IGenerator
    {
        readonly INameGenerator _firstNameGenerator;
        readonly int _numberOfProspectsToGenerate;
        readonly int _seasonId;

        public ProspectGenerator(INameGenerator firstNameGenerator, int numberOfProspectsToGenerate, int seasonId)
        {
            _firstNameGenerator = firstNameGenerator;
            _numberOfProspectsToGenerate = numberOfProspectsToGenerate;
            _seasonId = seasonId;
        }

        public void Generate()
        {
            for(int i = 0; i<_numberOfProspectsToGenerate; i++)
            {
                Prospect p = new Prospect(_firstNameGenerator.Generate(), "", _seasonId);
                FootballManagerDatabase.AddProspect(p);
            }
        }
    }

    public class Prospect
    {
        readonly string _firstName;
        readonly string _position;
        readonly int _seasonId;
        

        public int SeasonId
        {
            get { return _seasonId; }
        }

        public string FirstName
        {
            get { return _firstName; }
           
        }

        public Prospect(string firstName, string position, int seasonId)
        {
            _firstName = firstName;
            _position = position;
            _seasonId = seasonId;
            
        }

        public bool IsInSeason(int seasonId)
        {
            return _seasonId == seasonId;
        }
    }
}