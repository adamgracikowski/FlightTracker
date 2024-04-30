using ProjOb_24L_01180781.Database.SQL;
using ProjOb_24L_01180781.Database.SQL.Queries;

namespace ProjOb_24L_01180781.Tests
{
    public class QueryParserTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ParseDeleteQuery_ForValidQuery_ReturnsCorrectDeleteQuery(int index)
        {
            var (query, expected) = ValidDeleteQuery[index];
            var actual = QueryParser.ParseDeleteQuery(query);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ParseDeleteQuery_ForInvalidQuery_ReturnsNull(int index)
        {
            var (query, _) = InvalidDeleteQuery[index];
            var actual = QueryParser.ParseDeleteQuery(query);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ParseDisplayQuery_ForValidQuery_ReturnsCorrectDisplayQuery(int index)
        {
            var (query, expected) = ValidDisplayQuery[index];
            var actual = QueryParser.ParseDisplayQuery(query);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ParseDisplayQuery_ForInvalidQuery_ReturnsNull(int index)
        {
            var (query, _) = InvalidDisplayQuery[index];
            var actual = QueryParser.ParseDisplayQuery(query);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ParseAddQuery_ForValidQuery_ReturnsCorrectAddQuery(int index)
        {
            var (query, expected) = ValidAddQuery[index];
            var actual = QueryParser.ParseAddQuery(query);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ParseAddQuery_ForInvalidQuery_ReturnsNull(int index)
        {
            var (query, _) = InvalidAddQuery[index];
            var actual = QueryParser.ParseAddQuery(query);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ParseUpdateQuery_ForValidQuery_ReturnsCorrectUpdateQuery(int index)
        {
            var (query, expected) = ValidUpdateQuery[index];
            var actual = QueryParser.ParseUpdateQuery(query);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ParseUpdateQuery_ForInvalidQuery_ReturnsNull(int index)
        {
            var (query, _) = InvalidUpdateQuery[index];
            var actual = QueryParser.ParseUpdateQuery(query);
            Assert.Null(actual);
        }

        private static readonly List<(string Query, DeleteQuery?)> ValidDeleteQuery = new()
        {
            (
                "delete Crew",
                new DeleteQuery("Crew", [], [])
            ),
            (
                "delete Crew where ID = 12",
                new DeleteQuery("Crew",
                    [new("ID", "=", "12")],
                    [])
            ),
            (
                "delete Crew where ID = 12 and Age > 25",
                new DeleteQuery("Crew",
                    [new("ID", "=", "12"), new("Age", ">", "25")],
                    ["and"])
            ),
            (
                "delete Crew where ID = 12 and Age > 25 or Name = 'John'",
                new DeleteQuery("Crew",
                    [new("ID", "=", "12"), new("Age", ">", "25"), new("Name", "=", "John")],
                    ["and", "or"])
            ),
            (
                "delete Crew where ID = 12 or Phone != '123-456-7890'",
                new DeleteQuery("Crew",
                    [new("ID", "=", "12"), new("Phone", "!=", "123-456-7890")],
                    ["or"])
            ),
        };
        private static readonly List<(string Query, DeleteQuery?)> InvalidDeleteQuery = new()
        {
            ("delete", null),
            ("delete Crew where", null),
            ("delete Crew ID = 12", null),
            ("delete Crew where ID = 12 Age > 25", null),
            ("delete Crew where ID = 12 and Age > 25 or", null)
        };
        private static readonly List<(string Query, DisplayQuery?)> ValidDisplayQuery = new()
        {
            (
                "display ID from Crew",
                new DisplayQuery("Crew",
                    ["ID"],
                    [],
                    [])
            ),
            (
                "display ID, Name, Age from Crew",
                new DisplayQuery("Crew",
                    ["ID", "Name", "Age"],
                    [],
                    [])
            ),
            (
                "display ID, WorldPosition.Lat, WorldPosition.Long from Flight where ID > 10 and ID < 20",
                new DisplayQuery("Flight",
                    ["ID", "WorldPosition.Lat", "WorldPosition.Long"],
                    [new("ID", ">", "10"), new("ID", "<", "20")],
                    ["and"])
            ),
            (
                "display ID, Weight, Code from Cargo where ID > 10 or Weight < 20.123",
                new DisplayQuery("Cargo",
                    ["ID", "Weight", "Code"],
                    [new("ID", ">", "10"), new("Weight", "<", "20.123")],
                    ["or"])
            ),
            (
                "display ID, Origin, Target from Flight where WorldPosition.Lat < 75.123 and WorldPosition.Long > -53.345",
                new DisplayQuery("Flight",
                    ["ID", "Origin", "Target"],
                    [new("WorldPosition.Lat", "<", "75.123"), new("WorldPosition.Long", ">", "-53.345")],
                    ["and"])
            ),
            (
                "display * from Cargo",
                new DisplayQuery("Cargo",
                    ["*"],
                    [],
                    [])
            )
        };
        private static readonly List<(string Query, DisplayQuery?)> InvalidDisplayQuery = new()
        {
            ("display ID from", null),
            ("display from Crew", null),
            ("display ID WorldPosition.Lat from Flight", null),
            ("display ID, Weight, Code Cargo where ID > 10 or Weight < 20.123", null),
            ("display ID, Origin, Target from Flight where", null),
            ("display *, ID from Flight where", null)
        };
        private static readonly List<(string Query, AddQuery?)> ValidAddQuery = new()
        {
            (
                "add Crew new (ID = 123, Weight = 12.456)",
                new AddQuery("Crew",
                    [new("ID", "123"), new("Weight", "12.456")])
            ),
            (
                "add Crew new (Name = 'John', Age = 25)",
                new AddQuery("Crew",
                    [new("Name", "John"), new("Age", "25")])
            ),
            (
                "add Flight new (ID = 123, WorldPosition.Lat = -34.123, WorldPosition.Long = 123.34)",
                new AddQuery("Flight",
                    [new("ID", "123"), new("WorldPosition.Lat", "-34.123"), new("WorldPosition.Long", "123.34")])
            )
        };
        private static readonly List<(string Query, AddQuery?)> InvalidAddQuery = new()
        {
            ("add Flight new", null),
            ("add Flight (ID = 12)", null),
            ("Flight new (ID = 12, WorldPosition.Lat = -45.456", null),
            ("add Crew new (Age = 25, ID = 26", null),
            ("add Crew new Age = 25, ID = 26)", null),
            ("add Crew new (Age = 25, = 26", null)
        };
        private static readonly List<(string Query, UpdateQuery?)> ValidUpdateQuery = new()
        {
            (
                "update Cargo set (Weight = 123.1)",
                new UpdateQuery("Cargo",
                    [new("Weight", "123.1")],
                    [],
                    [])
            ),
            (
                "update Cargo set (Weight = 123.1, ID = 5)",
                new UpdateQuery("Cargo",
                    [new("Weight", "123.1"), new("ID", "5")],
                    [],
                    [])
            ),
            (
                "update Cargo set (Weight = 123.1, ID = 5) where ID = 12",
                new UpdateQuery("Cargo",
                    [new("Weight", "123.1"), new("ID", "5")],
                    [new("ID", "=", "12")],
                    [])
            ),
            (
                "update Crew set (Name = 'Jasmine', Age = 21, Phone = '123-456-7890') where ID = 12 and Name = 'Alice'",
                new UpdateQuery("Crew",
                    [new("Name", "Jasmine"), new("Age", "21"), new("Phone", "123-456-7890")],
                    [new("ID", "=", "12"), new("Name", "=", "Alice")],
                    ["and"])
            ),
        };
        private static readonly List<(string Query, UpdateQuery?)> InvalidUpdateQuery = new()
        {
            ("update set (Weight = 123.1)", null),
            ("update Cargo set", null),
            ("update Cargo set (Weight = , ID = 5) where ID = 12", null),
            ("update Crew set (Name = 'Jasmine, Age = 21, Phone = '123-456-7890') where ID = 12 and Name = 'Alice'", null),
            ("update Crew set (Name = 'Jasmine' Age = 21, Phone = '123-456-7890') where ID = 12 and Name = 'Alice'", null),
            ("update Crew set (Name = 'Jasmine', Age = 21, Phone = '123-456-7890'", null),
        };
    }
}
