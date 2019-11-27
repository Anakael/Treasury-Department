import React, {FC} from 'react';
import {Button} from "@material-ui/core";
import {connect, ConnectedProps} from "react-redux";
import {Dispatch} from "redux";
import {RootState} from "../../../store/rootReducer";
import {Link, Redirect} from 'react-router-dom'
import SignUpForm from "./SignUpForm";

const mapStateToProps = (state: RootState) => ({
	auth: state.auth
});

const mapDispatchToProps = (dispatch: Dispatch) => ({});

const connector = connect(
	mapStateToProps,
	mapDispatchToProps
);

type PropsFromRedux = ConnectedProps<typeof connector>

type SignUpProps = PropsFromRedux

export const SignUp: FC<SignUpProps> = (props) => {
	return (
		!!props.auth.token
			? <Redirect to="/"/>
			: <div>
				<SignUpForm/>
				<Link to={'/login'}> <Button>Log in</Button></Link>
			</div>
	);
};

export default connector(SignUp);
