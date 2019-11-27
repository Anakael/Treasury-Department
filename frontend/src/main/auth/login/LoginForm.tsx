import {Credentials} from "../../../models/login/Credentials";
import {withFormik} from 'formik';
import {RootState} from "../../../store/rootReducer";
import {Dispatch} from "redux";
import {logIn} from "../../../store/login/actions/loginActions";
import {connect, ConnectedProps} from "react-redux";
import {InnerForm} from "./LoginInnerForm";


const mapStateToProps = (state: RootState) => ({
	auth: state.auth
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
	logIn: (credentials: Credentials) => dispatch(logIn(credentials))
});

const connector = connect(
	mapStateToProps,
	mapDispatchToProps
);

type PropsFromRedux = ConnectedProps<typeof connector>

type LoginProps = PropsFromRedux

const LoginForm = withFormik<LoginProps, Credentials>({
	mapPropsToValues: props => {
		return {
			login: '',
			password: ''
		}
	},
	handleSubmit: (values, {props}) => {
		props.logIn(values);
	},
})(InnerForm);

export default connector(LoginForm);
