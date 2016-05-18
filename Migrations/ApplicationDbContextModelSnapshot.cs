using System;
using PersonalWebApp.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

            modelBuilder.Entity("PersonalWebApp.Models.Entity.BlogComment", b =>
                {
                    b.Property<Guid>("CommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorEmail")
                        .IsRequired();

                    b.Property<string>("AuthorName")
                        .IsRequired();

                    b.Property<string>("AuthorWebsite");

                    b.Property<DateTimeOffset?>("DateReviewed");

                    b.Property<DateTimeOffset>("DateSubmitted");

                    b.Property<Guid>("EntryId");

                    b.Property<string>("HtmlContent");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("MarkdownContent");

                    b.HasKey("CommentId");

                    b.HasAnnotation("Relational:Schema", "blog");

                    b.HasAnnotation("Relational:TableName", "Comment");
                });

            modelBuilder.Entity("PersonalWebApp.Models.Entity.BlogEntry", b =>
                {
                    b.Property<Guid>("EntryId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset>("DateLastModified");

                    b.Property<DateTimeOffset?>("DatePublished");

                    b.Property<string>("HtmlContent");

                    b.Property<string>("MarkdownContent");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 160);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 160);

                    b.HasKey("EntryId");

                    b.HasAnnotation("Relational:Schema", "blog");

                    b.HasAnnotation("Relational:TableName", "Entry");
                });

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

            modelBuilder.Entity("PersonalWebApp.Models.Entity.BlogComment", b =>
                {
                    b.HasOne("PersonalWebApp.Models.Entity.BlogEntry")
                        .WithMany()
                        .HasForeignKey("EntryId");
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
