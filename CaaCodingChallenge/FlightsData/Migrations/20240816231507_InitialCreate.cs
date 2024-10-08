﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightsData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Airline = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureAirport = table.Column<string>(type: "TEXT", nullable: false),
                    ArrivalAirport = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
