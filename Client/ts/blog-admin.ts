'use strict';

const signin = document.getElementById('signin');
const buttons = document.querySelectorAll('.buttons button');

function toggleButtons(show: boolean) {
	for (let i = 0; i < buttons.length; i++) {
		(buttons[i] as HTMLButtonElement).hidden = !show;
	}
}

toggleButtons(false);

function onSignIn(googleUser: gapi.auth2.GoogleUser) {
	const idToken = googleUser.getAuthResponse().id_token;
	var xhr = new XMLHttpRequest();
	xhr.open('POST', '/blog/auth');
	xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
	xhr.onload = () => {
		if (xhr.status === 200) {
			const obj = JSON.parse(xhr.responseText);
			console.log(`Signed in as ${obj.name}`);
			console.log(`Picture URL: ${obj.picture}`);
			console.log(`Email: ${obj.email}`);
			signin.hidden = true;
			toggleButtons(true);
		} else {
			console.log(`Request failed; returned status ${xhr.status}.`);
		}
	};
	xhr.send(`id_token=${idToken}`);
}

function onFailure(error: any) {
	console.log(error);
}

function renderButton() {
	gapi.signin2.render('signin', {
		scope: 'profile email',
		width: 240,
		height: 50,
		longtitle: true,
		theme: 'dark',
		onsuccess: onSignIn,
		onfailure: onFailure
	});
}