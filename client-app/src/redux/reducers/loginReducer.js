let user = JSON.parse(localStorage.getItem('user'));
const initialState = user ? { loggedIn: true, user } : {};

const login = (state = initialState, action) => {
    switch (action.type) {
        case 'LOGIN_REQUEST':
            return {
                loggingIn: true,
                user: state.user
            };
        case 'LOGIN_SUCCESS':
            return {
                loggedIn: true,
                user: state.user
            };
        case 'LOGIN_FAILURE':
            return {};
        case 'LOGOUT_USER':
            return {};
        default:
            return state
    }
}

export default login;