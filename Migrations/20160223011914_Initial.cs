using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema("skill");
            migrationBuilder.CreateTable(
                name: "Skill",
                schema: "skill",
                columns: table => new
                {
                    SkillId = table.Column<Guid>(nullable: false),
										Name = table.Column<string>(nullable: false),
										Code = table.Column<string>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.SkillId);
                });
            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "skill",
                columns: table => new
                {
                    TagId = table.Column<Guid>(nullable: false),
										Name = table.Column<string>(nullable: false),
										Code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTag", x => x.TagId);
                });
            migrationBuilder.CreateTable(
                name: "SkillToTag",
                schema: "skill",
                columns: table => new
                {
                    SkillId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillToTag", x => new { x.SkillId, x.TagId });
                    table.ForeignKey(
                        name: "FK_SkillToTag_Skill_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "skill",
                        principalTable: "Skill",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillToTag_SkillTag_TagId",
                        column: x => x.TagId,
                        principalSchema: "skill",
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_Skill_Code",
                schema: "skill",
                table: "Skill",
                column: "Code",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_SkillTag_Code",
                schema: "skill",
                table: "Tag",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SkillToTag", schema: "skill");
            migrationBuilder.DropTable(name: "Skill", schema: "skill");
            migrationBuilder.DropTable(name: "Tag", schema: "skill");
        }
    }
}
