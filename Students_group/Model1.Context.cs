﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Students_group
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Sudents_for_VorEntities : DbContext
    {
        private static Sudents_for_VorEntities sudents_For_Vor;
        //private static Sudents_for_VorEntities sudents_For_Vor;

        public Sudents_for_VorEntities()
            : base("name=Sudents_for_VorEntities")
        {

        }

        public static Sudents_for_VorEntities GetContext()
		{
            if (sudents_For_Vor == null)
                sudents_For_Vor = new Sudents_for_VorEntities();
            return sudents_For_Vor;
        }

        //public static Sudents_for_VorEntities GetContext()
        //{
        //    if (sudents_For_Vor == null)
        //        sudents_For_Vor = new Sudents_for_VorEntities();
        //    return sudents_For_Vor;
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Mentor> Mentor { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
