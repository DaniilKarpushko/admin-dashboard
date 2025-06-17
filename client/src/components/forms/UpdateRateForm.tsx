import { useState } from "react";
import api from "../../api/api";

const UpdateRateForm = () => {
    const [newRate, setNewRate] = useState<string>("");
    const [message, setMessage] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        const rate = parseFloat(newRate);
        if (isNaN(rate) || rate <= 0) {
            setMessage("Please enter a valid positive number.");
            return;
        }

        try {
            await api.post("/rates/update", null, {
                params: { newRate: rate.toFixed(2) },
            });
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="max-w-sm mx-auto mt-10 space-y-4">
            <div>
                <input
                    type="number"
                    step="0.01"
                    min="0"
                    value={newRate}
                    onChange={(e) => setNewRate(e.target.value)}
                    className="w-full p-2 border bg-white rounded text-black"
                    placeholder="Enter new rate (e.g. 1.25)"
                    required
                />
            </div>
            <button
                type="submit"
                className="px-4 py-2 border border-white rounded text-white disabled:opacity-50 transition"
            >
                Update Rate
            </button>

            {message && (
                <div className="text-sm mt-2 text-center text-black">{message}</div>
            )}
        </form>
    );
};

export default UpdateRateForm;