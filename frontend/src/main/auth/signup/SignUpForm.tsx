import {withFormik} from 'formik';
import {RootState} from "../../../store/rootReducer";
import {Dispatch} from "redux";
import {signUp} from "../../../store/login/actions/loginActions";
import {connect, ConnectedProps} from "react-redux";
import {InnerForm, SignUpValues} from "./SignUpInnerForm";
import {SignUpRequest} from "../../../models/login/SignUpRequest";
import * as Yup from 'yup';

const SignUpSchema = Yup.object().shape({
	confirmPassword: Yup.string().oneOf([Yup.ref('credentials.password'), ''], 'Passwords must match')
});

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
	validationSchema: SignUpSchema,
	handleSubmit: (values, {props}) => {
		props.signUp({credentials: values.credentials, email: values.email});
	}
})(InnerForm);

export default connector(SignUpForm);
