import {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import axios from "axios";

const LoginForm = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            const response = await axios.post("http://localhost:5000/api/auth/login", {
                email,
                password,
            }, {
                withCredentials: true,
            });

            if (response.status === 200) {
                const username = response.data.username;
                const email = response.data.email;

                localStorage.setItem('username', username);
                localStorage.setItem('email', email);

                navigate('/dashboard');
            }
        } catch (error) {
            console.log(error);
        }
    }
    return (
        <form className="max-w-sm mx-auto" onSubmit={handleSubmit}>
            <div className="mb-5">
                <label htmlFor="email" className="float-left mb-2 text-sm font-medium">Your email</label>
                <input type="email" id="email"
                       className="w-full p-2.5 border-b-2 border-b-black focus:border-b-white focus:outline-none transition-colors duration-300 ease-in-out"
                       placeholder="email@example.com" onChange={(e) => setEmail(e.target.value)} required/>
            </div>
            <div className="mb-5">
                <label htmlFor="password" className="float-left mb-2 text-sm font-medium">Your
                    password</label>
                <input type="password" id="password"
                       className="w-full p-2.5 border-b-2 border-b-black focus:border-b-white focus:outline-none transition-colors duration-300 ease-in-out"
                       placeholder="password"
                       onChange={(e) => setPassword(e.target.value)} required/>
            </div>
            <button type="submit">Login</button>
        </form>
    )
}

export default LoginForm;