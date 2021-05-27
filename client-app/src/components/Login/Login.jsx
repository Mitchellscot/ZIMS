import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import './Login.css';
import {useSelector} from 'react-redux';


export default function Login() {
    const [inputs, setInputs] = useState({
        username: '',
        password: ''
    });
    const [submitted, setSubmitted] = useState(false);
    const { username, password } = inputs;
    const dispatch = useDispatch();

     useEffect(() => {
        dispatch({type: 'LOGOUT'});
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setInputs(inputs => ({ ...inputs, [name]: value }));
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        setSubmitted(true);
        if (username && password) {
            //get return url from location state or default to home page
            //const { from } = location.state || { from: { pathname: "/" } };
            dispatch({type: 'LOGIN', payload: {
                username: inputs.username,
                password: inputs.password,
            },});
/*             else{
                //dispatch LOGIN FAILURE or something to trigger alert messages
            } */
        }
    }


    return (
        <div className="container">
        <div className="col-md-8 offset-md-2 mt-5">
        <div className="col-lg-8 offset-lg-2 border rounded">
        <div className="d-flex justify-content-center navbar-brand mt-3">
            <img src="ZIMS-logo.png" height="400px" width="363px" alt="logo"/>
            </div>
            <form name="form" onSubmit={handleSubmit}>
                <div className="form-row d-flex">
                    <div className="col mx-2">
                    <input placeholder="username" className="form-control" id="usernameInput" required type="text" name="username" value={username} onChange={handleChange} /></div>
                    {submitted && !username && <div className="invalid-feedback">Username is required</div>}
                    <div className="col mx-2">
                    <input placeholder="password" className="form-control" id="passwordInput" required type="text" name="password" value={password} onChange={handleChange} />
                    {submitted && !password && <div className="invalid-feedback">Password is required</div>}
                </div>
                </div>
                <div className="form-group d-flex justify-content-between m-2">
                    <button className="btn btn-primary btn" type="submit" name="submit">
                    {/*loggingIn && <span className="spinner-border spinner-border-sm mr-1"></span>*/}
                        Login
                    </button>

                    <button
                    onClick={(e) => alert("too bad")}
                    id="forgotPassword" type="button" className="btn btn-link btn">Forgot password?</button>
                </div>
            </form>
            </div>
            </div>
            </div>
    );
}