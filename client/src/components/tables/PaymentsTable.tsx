import { useEffect, useState } from "react";
import { Payment } from "../../models/Payment";
import api from "../../api/api";

const PaymentsTable = () => {
    const [payments, setPayments] = useState<Payment[]>([]);

    const fetchPayments = async () => {
        try {
            const response = await api.get("/payments", {
                params: { take: 5, cursor: 0 },
            });

            setPayments(response.data.payments);
        } catch (error) {
            console.error("Failed to fetch payments", error);
        }
    };

    useEffect(() => {
        fetchPayments();
    }, []);

    return (
        <div className="min-w-3xl mx-auto mt-10">
            <table className="w-full bg-white rounded">
                <thead>
                <tr>
                    <th className="p-2 text-black text-center">ID</th>
                    <th className="p-2 text-black text-center">Client</th>
                    <th className="p-2 text-black text-center">Total</th>
                    <th className="p-2 text-black text-center">Date</th>
                </tr>
                </thead>
                <tbody>
                {payments.map((payment) => (
                    <tr key={payment.id} className="border-t">
                        <td className="p-2 text-center text-black">{payment.id}</td>
                        <td className="p-2 text-center text-black">{payment.clientName}</td>
                        <td className="p-2 text-center text-black">
                            {payment.total.toFixed(2)}
                        </td>
                        <td className="p-2 text-center text-black">
                            {new Date(payment.createdAt).toLocaleString()}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default PaymentsTable;
