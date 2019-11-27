import {Redirect, Route} from "react-router";
import Login from "./main/auth/login/Login";
import SignUp from "./main/auth/signup/SignUp";
import MainPage from "./main/main_page/MainPage";
import {BrowserRouter} from "react-router-dom";
import React, {FC} from "react";
import {RootState} from "./store/rootReducer";
import {connect, ConnectedProps} from "react-redux";

const mapStateToProps = (state: RootState) => ({
	isAuthenticated: !!state.auth.token
});

const connector = connect(
	mapStateToProps,
	{}
);

const PrivateRoute = ({component: Component, isAuthenticated, ...rest}) => (
	<Route {...rest} render={(props) => (
		isAuthenticated
			? <Component {...props} />
			: <Redirect to={{
				pathname: '/login'
			}}/>
	)}/>
);

type PropsFromRedux = ConnectedProps<typeof connector>

const Router: FC<PropsFromRedux> = (props: PropsFromRedux) => {
	return (
		<BrowserRouter>
				<Route path="/login" component={Login}/>
				<Route path="/signup" component={SignUp}/>
				<PrivateRoute path='/' exact component={MainPage} isAuthenticated={props.isAuthenticated}/>
		</BrowserRouter>
	)
};

export default connector(Router);
