import React, {Component, ReactNode} from "react";
import {RootState} from "../../store/rootReducer";
import {connect, ConnectedProps} from "react-redux";

const mapStateToProps = (state: RootState) => ({
	auth: state.auth
});

const connector = connect(
	mapStateToProps,
	{}
);

type PropsFromRedux = ConnectedProps<typeof connector>

type MainPageProps = PropsFromRedux

class MainPage extends Component<MainPageProps, {}> {
	render(): ReactNode {
		return (
			<div>
				Hello {this.props.auth.user.login}!
			</div>
		)
	}
}

export default connector(MainPage);
