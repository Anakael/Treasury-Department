import React, {FC} from 'react';
import {BrowserRouter as Router, Redirect, Route} from 'react-router-dom'
import {RootState} from "./store/rootReducer";
import {connect, ConnectedProps} from "react-redux";
import Login from "./main/auth/login/Login";
import MainPage from "./main/main_page/MainPage";

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

const App: FC<PropsFromRedux> = (props: PropsFromRedux) => {
	return (
		<div className="App">
			<Router>
				<header className="App-header">
					<Route path="/login" component={Login}/>
					<PrivateRoute path='/' exact component={MainPage} isAuthenticated={props.isAuthenticated}/>
				</header>
			</Router>
		</div>
	);
};

export default connector(App);
