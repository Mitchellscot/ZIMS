import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './components/App/App.jsx';
import { createStore, applyMiddleware } from 'redux';
import { Provider } from 'react-redux';
import createSagaMiddleware from 'redux-saga';
import rootReducer from './redux/reducers/_rootReducer';
import rootSaga from './redux/sagas/_rootSaga';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

const sagaMiddleware = createSagaMiddleware();

const store = createStore(
    rootReducer,
    applyMiddleware(sagaMiddleware),
);

sagaMiddleware.run(rootSaga);

ReactDOM.render(

    <BrowserRouter basename={baseUrl}>
        <Provider store={store}>
          <React.StrictMode>
          <App />
          </React.StrictMode>
        </Provider>
    </BrowserRouter>
    ,
    rootElement);