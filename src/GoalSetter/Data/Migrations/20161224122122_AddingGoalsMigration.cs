﻿// <auto-generated/>

namespace GoalSetter.Migrations
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// AddingGoalsMigration
    /// </summary>
    public partial class AddingGoalsMigration : Migration
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Up(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    GoalId = table.Column<Guid>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => new { x.UserId, x.GoalId });
                });
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            migrationBuilder.DropTable(
                name: "Goals");
        }
    }
}
