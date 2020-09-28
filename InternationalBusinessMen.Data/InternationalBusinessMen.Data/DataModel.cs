namespace InternationalBusinessMen.Data
{
    using InternationalBusinessMen.Data.Model;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DataModel : DbContext
    {
        public DataModel() : base("name=DataModel")
        {
        }

        public virtual DbSet<Transaction> Transactions { get;set; }
        public virtual DbSet<Conversion> Conversions { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }


        //public class MyEntity
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //}
    }

}