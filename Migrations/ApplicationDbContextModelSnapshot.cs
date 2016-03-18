using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using PersonalWebApp.Models.Db;

namespace PersonalWebApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PersonalWebApp.Models.Entity.Skill", b =>
                {
                    b.Property<Guid>("SkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("IconClass")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("Rating");

                    b.HasKey("SkillId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasAnnotation("Relational:Schema", "skill");

                    b.HasAnnotation("Relational:TableName", "Skill");
                });

            modelBuilder.Entity("PersonalWebApp.Models.Entity.SkillTag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("TagId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasAnnotation("Relational:Schema", "skill");

                    b.HasAnnotation("Relational:TableName", "Tag");
                });

            modelBuilder.Entity("PersonalWebApp.Models.Entity.SkillToTag", b =>
                {
                    b.Property<Guid>("SkillId");

                    b.Property<Guid>("TagId");

                    b.HasKey("SkillId", "TagId");

                    b.HasAnnotation("Relational:Schema", "skill");

                    b.HasAnnotation("Relational:TableName", "SkillToTag");
                });

            modelBuilder.Entity("PersonalWebApp.Models.Entity.SkillToTag", b =>
                {
                    b.HasOne("PersonalWebApp.Models.Entity.Skill")
                        .WithMany()
                        .HasForeignKey("SkillId");

                    b.HasOne("PersonalWebApp.Models.Entity.SkillTag")
                        .WithMany()
                        .HasForeignKey("TagId");
                });
        }
    }
}
