import {useState} from "react";
import {RateHistoryItem} from "../../models/RateHistoryItem";
import api from "../../api/api";

const RateHistoryTable = () => {
    const [rates, setRates] = useState<RateHistoryItem[]>([]);
    const [fromDate, setFromDate] = useState("2024-01-01");
    const [toDate, setToDate] = useState("2025-12-31");

    const fetchRates = async () => {
        try {
            const response = await api.get("/rates", {
                params: {
                    from: new Date(fromDate).toISOString(),
                    to: new Date(toDate).toISOString(),
                },
            });

            setRates(response.data.rates);
        } catch (error) {
            console.error("Failed to fetch rates", error);
        }
    };

    return (
        <div className="max-w-4xl mx-auto mt-10">
            <div className="flex gap-4 mb-4 items-end">
                <label className="text-white">
                    From:
                    <input
                        type="date"
                        value={fromDate}
                        onChange={(e) => setFromDate(e.target.value)}
                        className="px-4 py-2 border text-black bg-white rounded"
                    />
                </label>
                <label className="text-white">
                    To:
                    <input
                        type="date"
                        value={toDate}
                        onChange={(e) => setToDate(e.target.value)}
                        className="px-4 py-2 border text-black bg-white rounded"
                    />
                </label>
                <button
                    onClick={fetchRates}
                >
                    Load
                </button>
            </div>

            <table className="w-full bg-white rounded">
                <thead>
                <tr>
                    <th className="p-2 text-center text-black">Rate</th>
                    <th className="p-2 text-center text-black">Created At</th>
                </tr>
                </thead>
                <tbody>
                {rates.map((rate) => (
                    <tr key={rate.id} className="border-t">
                        <td className="p-2 text-center text-black">{rate.rate.toFixed(2)}</td>
                        <td className="p-2 text-center text-black">
                            {new Date(rate.createdAt).toLocaleString()}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default RateHistoryTable;

