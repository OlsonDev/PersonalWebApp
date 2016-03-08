using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace PersonalWebApp.Migrations
{
    public partial class AddSkillIconClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_SkillToTag_Skill_SkillId", schema: "skill", table: "SkillToTag");
            migrationBuilder.DropForeignKey(name: "FK_SkillToTag_SkillTag_TagId", schema: "skill", table: "SkillToTag");
            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                schema: "skill",
                table: "Skill",
                nullable: false,
                defaultValue: "");
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
            migrationBuilder.DropColumn(name: "IconClass", schema: "skill", table: "Skill");
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
