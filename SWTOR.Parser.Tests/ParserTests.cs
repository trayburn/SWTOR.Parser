using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SWTOR.Parser.Tests
{
	[TestFixture()]
	public class OneRow_ParserTests
	{
		private StringBuilder oneRow;
		private Parser target;
		private StringReader rdr;
		
		[SetUp]
		public void SetUp()
		{
			target = new Parser();
			oneRow = new StringBuilder();
			oneRow.Append("[03/01/2012 14:35:20] [@Idrurrez] [@Khantni] " + 
			              "[Force Clap {2848585519464448}] " + 
			              "[ApplyEffect {836045448945477}: Stunned (Physical) {2848585519464704}] ()");
			rdr = new StringReader(oneRow.ToString());
		}
		
		[Test]
		public void Parse_Should_Return_Player_Idrurrez_As_Source()
		{
			// Arrange
			
			// Act
			var list = target.Parse(rdr);
			
			// Assert
			var entry = list.First();
			Assert.AreEqual("Idrurrez", entry.Source.Name);
			Assert.AreEqual(true, entry.Source.IsPlayer);
		}
		
				[Test()]
		public void Parse_Should_Return_One_Entry()
		{
			// Arrange
			
			// Act
			var list = target.Parse(rdr);
			
			// Assert
			Assert.AreEqual(1,list.Count);
		}
		
		[Test]
		public void Parse_Should_Return_Correct_Timestamp()
		{
			// Arrange
			
			// Act
			var list = target.Parse(rdr);
			
			// Assert
			var entry = list.First();
			var expected = new DateTime(2012,3,1,14,35,20,DateTimeKind.Unspecified);
			Assert.AreEqual(expected, entry.Timestamp);
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Parse_Should_Throw_When_TextReader_Is_Null ()
		{
			// Arrange
			
			// Act
			var list = target.Parse(null);
			
			// Assert
			Assert.IsNull(list);
			Assert.Fail();
		}		
	}
}

