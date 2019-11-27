import {withFormik} from 'formik';
import {RootState} from "../../../store/rootReducer";
import {Dispatch} from "redux";
import {signUp} from "../../../store/login/actions/loginActions";
import {connect, ConnectedProps} from "react-redux";
import {InnerForm, SignUpValues} from "./SignUpInnerForm";
import {SignUpRequest} from "../../../models/login/SignUpRequest";


const mapStateToProps = (state: RootState) => ({
	auth: state.auth
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
	signUp: (request: SignUpRequest) => dispatch(signUp(request))
});

const connector = connect(
	mapStateToProps,
	mapDispatchToProps
);

type PropsFromRedux = ConnectedProps<typeof connector>

type SignUpProps = PropsFromRedux

const SignUpForm = withFormik<SignUpProps, SignUpValues>({
	mapPropsToValues: props => {
		return {
			credentials: {
				login: '',
				password: ''
			},
			email: '',
			confirmPassword: ''
		}
	},
	handleSubmit: (values, {props}) => {
		props.signUp(values);
	},
})(InnerForm);

export default connector(SignUpForm);
