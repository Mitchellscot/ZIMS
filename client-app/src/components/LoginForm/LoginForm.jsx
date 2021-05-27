import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';

function LoginForm({setForgotPassword}) {
    const alerts = useSelector(store => store.loginAlert)
    const loggingIn = useSelector(store => store.login.loggingIn);
    const dispatch = useDispatch();
    const [inputs, setInputs] = useState({
        username: '',
        password: ''
    });
    const { username, password } = inputs;
    const [submitted, setSubmitted] = useState(false);


    const handleChange = (e) => {
        const { name, value } = e.target;
        setInputs(inputs => ({ ...inputs, [name]: value }));
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        setSubmitted(true);
        if (username && password) {

            dispatch({
                type: 'LOGIN', payload: {
                    username: inputs.username,
                    password: inputs.password,
                },
            });
        }
        else {
            dispatch({ type: 'ALERT_CLEAR' });
        }
    }

    /*     useEffect(() => {
    
        }, []); */

    return (

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
                    {loggingIn && <span className="spinner-border spinner-border-sm mr-1"></span>}
                        Login
                    </button>
                <button
                    onClick={() => setForgotPassword(true)}
                    id="forgotPassword" type="button" className="btn btn-link btn">Forgot password?</button>
            </div>
        </form>

    );
}

export default LoginForm;