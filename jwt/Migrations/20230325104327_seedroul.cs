using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace jwt.Migrations
{
    public partial class seedroul : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table:"AspNetRoles",
                columns:new [] {"Id","Name" , "NormalizedName" , "ConcurrencyStamp"} ,
                values: new object [] {Guid.NewGuid().ToString() , "Users", "Users".ToUpper(), Guid.NewGuid().ToString() }
                );
            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }
               );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from AspNetRoles ");
        }
    }
}
