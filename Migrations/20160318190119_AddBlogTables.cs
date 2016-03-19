using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace PersonalWebApp.Migrations
{
    public partial class AddBlogTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_SkillToTag_Skill_SkillId", schema: "skill", table: "SkillToTag");
            migrationBuilder.DropForeignKey(name: "FK_SkillToTag_SkillTag_TagId", schema: "skill", table: "SkillToTag");
            migrationBuilder.EnsureSchema("blog");
            migrationBuilder.CreateTable(
                name: "Entry",
                schema: "blog",
                columns: table => new
                {
                    EntryId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    DateLastModified = table.Column<DateTimeOffset>(nullable: false),
                    DatePublished = table.Column<DateTimeOffset>(nullable: true),
                    HtmlContent = table.Column<string>(nullable: true),
                    MarkdownContent = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogEntry", x => x.EntryId);
                });
            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "blog",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(nullable: false),
                    AuthorEmail = table.Column<string>(nullable: false),
                    AuthorName = table.Column<string>(nullable: false),
                    AuthorWebsite = table.Column<string>(nullable: true),
                    DateReviewed = table.Column<DateTimeOffset>(nullable: true),
                    DateSubmitted = table.Column<DateTimeOffset>(nullable: false),
                    EntryId = table.Column<Guid>(nullable: false),
                    HtmlContent = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    MarkdownContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_BlogComment_BlogEntry_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "blog",
                        principalTable: "Entry",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.AddForeignKey(
                name: "FK_SkillToTag_Skill_SkillId",
                schema: "skill",
                table: "SkillToTag",
                column: "SkillId",
                principalSchema: "skill",
                principalTable: "Skill",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_SkillToTag_SkillTag_TagId",
                schema: "skill",
                table: "SkillToTag",
                column: "TagId",
                principalSchema: "skill",
                principalTable: "Tag",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_SkillToTag_Skill_SkillId", schema: "skill", table: "SkillToTag");
            migrationBuilder.DropForeignKey(name: "FK_SkillToTag_SkillTag_TagId", schema: "skill", table: "SkillToTag");
            migrationBuilder.DropTable(name: "Comment", schema: "blog");
            migrationBuilder.DropTable(name: "Entry", schema: "blog");
            migrationBuilder.AddForeignKey(
                name: "FK_SkillToTag_Skill_SkillId",
                schema: "skill",
                table: "SkillToTag",
                column: "SkillId",
                principalSchema: "skill",
                principalTable: "Skill",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_SkillToTag_SkillTag_TagId",
                schema: "skill",
                table: "SkillToTag",
                column: "TagId",
                principalSchema: "skill",
                principalTable: "Tag",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
