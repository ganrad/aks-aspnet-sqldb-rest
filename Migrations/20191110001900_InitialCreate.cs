using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClaimsApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ClaimNumbers",
                startValue: 100L);

            migrationBuilder.CreateSequence<int>(
                name: "PlanPayNumbers",
                startValue: 100L);

            migrationBuilder.CreateSequence<int>(
                name: "ServiceLineNumbers",
                startValue: 100L);

            migrationBuilder.CreateSequence<int>(
                name: "SubNumbers",
                startValue: 100L);

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    claim_item_id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR ClaimNumbers"),
                    claim_status = table.Column<string>(nullable: true),
                    claim_type = table.Column<string>(nullable: true),
                    sender_id = table.Column<string>(nullable: true),
                    receiver_id = table.Column<string>(nullable: true),
                    originator_id = table.Column<string>(nullable: true),
                    destination_id = table.Column<string>(nullable: true),
                    claim_inp_method = table.Column<string>(nullable: true),
                    claim_no = table.Column<string>(nullable: true),
                    total_claim_charge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    patient_status = table.Column<string>(nullable: true),
                    patient_amt_due = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    service_date = table.Column<DateTime>(nullable: false),
                    policy_no = table.Column<string>(nullable: true),
                    claim_paid_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.claim_item_id);
                });

            migrationBuilder.CreateTable(
                name: "Plan_Payment",
                columns: table => new
                {
                    plan_payment_id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR PlanPayNumbers"),
                    primary_payer_id = table.Column<string>(nullable: true),
                    cob_service_paid_amt = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    service_code = table.Column<string>(nullable: true),
                    payment_date = table.Column<DateTime>(nullable: false),
                    claim_adj_grp_code = table.Column<string>(nullable: true),
                    claim_adj_reason_code = table.Column<string>(nullable: true),
                    claim_adj_amt = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    claim_adj_qty = table.Column<string>(nullable: true),
                    claim_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan_Payment", x => x.plan_payment_id);
                    table.ForeignKey(
                        name: "FK_Plan_Payment_Claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "Claims",
                        principalColumn: "claim_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriber_Info",
                columns: table => new
                {
                    subscriber_id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR SubNumbers"),
                    subscriber_reln = table.Column<string>(nullable: true),
                    subscriber_policy_no = table.Column<string>(nullable: true),
                    insured_group_name = table.Column<string>(nullable: true),
                    subscriber_last_name = table.Column<string>(nullable: false),
                    subscriber_first_name = table.Column<string>(nullable: false),
                    subscriber_middle_name = table.Column<string>(nullable: true),
                    subscriber_id_ssn = table.Column<string>(nullable: false),
                    subscriber_addr_1 = table.Column<string>(nullable: true),
                    subscriber_address_2 = table.Column<string>(nullable: true),
                    subscriber_city = table.Column<string>(nullable: true),
                    subscriber_state = table.Column<string>(nullable: true),
                    subscriber_postal_code = table.Column<string>(nullable: true),
                    subscriber_country = table.Column<string>(nullable: true),
                    subscriber_dob = table.Column<string>(nullable: true),
                    subscriber_gender = table.Column<string>(nullable: true),
                    payer_name = table.Column<string>(nullable: false),
                    patient_last_name = table.Column<string>(nullable: true),
                    patient_first_name = table.Column<string>(nullable: true),
                    patient_ssn = table.Column<string>(nullable: true),
                    patient_member_id = table.Column<string>(nullable: true),
                    patient_dob = table.Column<string>(nullable: true),
                    patient_gender = table.Column<string>(nullable: true),
                    category_of_service = table.Column<string>(nullable: true),
                    claim_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber_Info", x => x.subscriber_id);
                    table.ForeignKey(
                        name: "FK_Subscriber_Info_Claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "Claims",
                        principalColumn: "claim_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Svc_Line_Details",
                columns: table => new
                {
                    service_line_id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR ServiceLineNumbers"),
                    statement_date = table.Column<DateTime>(nullable: false),
                    line_counter = table.Column<int>(nullable: false),
                    service_code_desc = table.Column<string>(nullable: true),
                    line_charge_amt = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    drug_code = table.Column<string>(nullable: true),
                    drug_unit_qty = table.Column<int>(nullable: false),
                    pharmacy_pres_no = table.Column<string>(nullable: true),
                    service_type = table.Column<string>(nullable: false),
                    provider_code = table.Column<string>(nullable: true),
                    provider_last_name = table.Column<string>(nullable: true),
                    provider_first_name = table.Column<string>(nullable: true),
                    provider_id = table.Column<string>(nullable: true),
                    in_network_id = table.Column<bool>(nullable: false),
                    claim_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Svc_Line_Details", x => x.service_line_id);
                    table.ForeignKey(
                        name: "FK_Svc_Line_Details_Claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "Claims",
                        principalColumn: "claim_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plan_Payment_claim_id",
                table: "Plan_Payment",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_Info_claim_id",
                table: "Subscriber_Info",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "IX_Svc_Line_Details_claim_id",
                table: "Svc_Line_Details",
                column: "claim_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plan_Payment");

            migrationBuilder.DropTable(
                name: "Subscriber_Info");

            migrationBuilder.DropTable(
                name: "Svc_Line_Details");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropSequence(
                name: "ClaimNumbers");

            migrationBuilder.DropSequence(
                name: "PlanPayNumbers");

            migrationBuilder.DropSequence(
                name: "ServiceLineNumbers");

            migrationBuilder.DropSequence(
                name: "SubNumbers");
        }
    }
}
