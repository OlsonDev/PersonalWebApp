using System;
using PersonalWebApp.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PersonalWebApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160308190454_AddSkillIconClass")]
    partial class AddSkillIconClass
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
