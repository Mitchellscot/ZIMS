import { put, takeLatest } from 'redux-saga/effects';
import axios from 'axios';

function* loginUser(action){

    try {
        //yield put({type: 'CLEAR_LOGIN_ERROR' }) // or something like that
        const config = {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true,
          };
          let user = {};
        yield axios.post('/user/authenticate', action.payload, config).then(response => {
                  localStorage.setItem('user', JSON.stringify(response.data));
                  user = response.data;
              });
        yield put({type: 'LOGIN_SUCCESS', user: user});

    } catch (error) {
        console.log('HEY MITCH - ERROR LOGGING IN', error);
        if (error.response.status === 401){
            yield put({ type: 'LOGIN_FAILURE' });
        }else{
            //yield put({ type: 'LOGIN_FAILED_NO_CODE' });
        }
    }
}

function* logoutUser(){
    try {
        localStorage.removeItem('user');
        yield put({type: 'LOGOUT_USER'});
    } catch (error) {
        console.log('HEY MITCH - COULD NOT LOG OUT USER', error);
    }
}

function* loginSaga(){
    yield takeLatest('LOGIN', loginUser);
    yield takeLatest('LOGOUT', logoutUser);
}

export default loginSaga;