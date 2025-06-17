import ClientsTable from "../components/tables/ClientsTable";
import PaymentsTable from "../components/tables/PaymentsTable";
import RateHistoryTable from "../components/tables/RateHistoryTable";
import UpdateRateForm from "../components/forms/UpdateRateForm";

const DashboardPage = () => {
    return (
        <div className="pt-0">
            <div>
                <h1 className="font-bold text-6xl">Clients</h1>
                <ClientsTable/>
            </div>
            <div className="pt-4">
                <h1 className="font-bold text-6xl">Payments</h1>
                <PaymentsTable/>
            </div>
            <div className="pt-4">
                <h1 className="font-bold text-6xl">Rate</h1>
                <div className="flex">
                    <RateHistoryTable/>
                    <UpdateRateForm/>
                </div>
            </div>
        </div>
    )
}

export default DashboardPage;