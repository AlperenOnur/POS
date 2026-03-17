using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;

#nullable enable

namespace DataModel
{
    public partial class OsterStadtDb : DataConnection
    {
        // SQLite direkt, ohne App.config, funktioniert in v6.2.0
        public OsterStadtDb()
            : base(SQLiteTools.GetDataProvider(), "Data Source=OsterStadt.db")
        {
            InitDataContext();
        }

        partial void InitDataContext();

        public ITable<Person> People => this.GetTable<Person>();
    }
}