using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SC.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 100, nullable: false),
                    DateOpened = table.Column<DateTime>(nullable: false),
                    State = table.Column<byte>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    DeviceName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketNumber);
                });

            migrationBuilder.CreateTable(
                name: "TicketResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsClientResponse = table.Column<bool>(nullable: false),
                    TicketFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketResponses_Tickets_TicketFK",
                        column: x => x.TicketFK,
                        principalTable: "Tickets",
                        principalColumn: "TicketNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketResponses_TicketFK",
                table: "TicketResponses",
                column: "TicketFK");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_State",
                table: "Tickets",
                column: "State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketResponses");

            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
