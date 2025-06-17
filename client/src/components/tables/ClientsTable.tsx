import { useEffect, useState } from "react";
import { Client } from "../../models/Client";
import api from "../../api/api";

const LIMIT = 5;

const ClientTable = () => {
    const [clients, setClients] = useState<Client[]>([]);
    const [cursor, setCursor] = useState<number | null>(null);
    const [prevStack, setPrevStack] = useState<number[]>([]);

    const fetchClients = async (cursor: number | null) => {
        try {
            const response = await api.get("/clients", {
                params: { limit: LIMIT, cursor: cursor },
            });

            setClients(response.data.clients);
        } catch (error) {
            console.error("Failed to fetch clients", error);
        }
    };

    useEffect(() => {
        fetchClients(cursor);
    }, [cursor]);

    const handleNext = () => {
        if (clients.length > 0) {
            setPrevStack((prev) => [...prev, clients[0].id]);
            setCursor(clients[clients.length - 1].id);
        }
    };

    const handlePrev = () => {
        if (prevStack.length > 0) {
            const prevCursor = prevStack[prevStack.length - 1];
            setPrevStack((stack) => stack.slice(0, -1));
            setCursor(prevCursor - 1);
        } else {
            setCursor(null);
        }
    };

    return (
        <div className="min-w-3xl mx-auto mt-10">
            <table className="w-full bg-white rounded">
                <thead>
                <tr>
                    <th className="p-2 text-black text-center">ID</th>
                    <th className="text-center p-2 text-black">Name</th>
                    <th className="text-center p-2 text-black">Email</th>
                    <th className="text-center p-2 text-black">BalanceT</th>
                </tr>
                </thead>
                <tbody>
                {clients.map((client) => (
                    <tr key={client.id} className="border-t">
                        <td className="p-2 text-center text-black">{client.id}</td>
                        <td className="p-2 text-center text-black">{client.name}</td>
                        <td className="p-2 text-center text-black">{client.email}</td>
                        <td className="p-2 text-center text-black">{client.balanceT.toFixed(2)}</td>
                    </tr>
                ))}
                </tbody>
            </table>

            <div className="flex justify-between mt-4">
                <button
                    onClick={handlePrev}
                    disabled={prevStack.length === 0}
                    className="disabled:opacity-50"
                >
                    Prev
                </button>
                <button
                    onClick={handleNext}
                    disabled={clients.length < LIMIT}
                    className="disabled:opacity-50"
                >
                    Next
                </button>
            </div>
        </div>
    );
};

export default ClientTable;
