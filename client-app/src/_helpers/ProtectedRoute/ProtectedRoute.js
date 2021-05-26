import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import Login from '../../components/Login/Login';
import {useSelector} from 'react-redux';

function ProtectedRoute(props) {
  const user = useSelector((store) => store.user);
  const {
    authRedirect,
    ...otherProps
  } = props;
  const ComponentToProtect = props.component || (() => props.children);

  let ComponentToShow;

  if (user) {
    ComponentToShow = ComponentToProtect;
  } else {
    ComponentToShow = Login;
  }

  if (user && authRedirect != null) {
    return <Redirect exact from={otherProps.path} to={authRedirect} />;
  } else if (!user && authRedirect != null) {
    ComponentToShow = ComponentToProtect;
  }

  return (
    <Route
      {...otherProps}
    >
      <ComponentToShow />
    </Route>

  );
}

export default ProtectedRoute;
