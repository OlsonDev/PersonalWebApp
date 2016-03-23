'use strict';

const signin           = document.getElementById('signin')                as HTMLDivElement;
const saveBtn          = document.getElementById('save-button')           as HTMLButtonElement;
const updatePreviewBtn = document.getElementById('update-preview-button') as HTMLButtonElement;
const form             = document.getElementById('blog-form')             as HTMLFormElement;

const buttons = document.querySelectorAll('.buttons button') as NodeListOf<HTMLButtonElement>;

function toggleButtons(show: boolean) {
	for (let i = 0; i < buttons.length; i++) {
		buttons[i].hidden = !show;
	}
}

toggleButtons(false);

function onSignIn(googleUser: gapi.auth2.GoogleUser) {
	const idToken = googleUser.getAuthResponse().id_token;
	const xhr = new XMLHttpRequest();
	xhr.open('POST', '/blog/auth');
	xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
	xhr.onload = () => {
		if (xhr.status === 200) {
			const obj = JSON.parse(xhr.responseText).result;
			console.log(`Signed in as ${obj.name}`);
			console.log(`Picture URL: ${obj.picture}`);
			console.log(`Email: ${obj.email}`);
			signin.hidden = true;
			toggleButtons(true);
		} else {
			console.log(`/blog/auth request failed; returned status ${xhr.status}.`);
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

updatePreviewBtn.addEventListener('click', () => {
	const data = new FormData(form);
	const xhr = new XMLHttpRequest();
	xhr.open('POST', '/blog/preview');
	xhr.onload = () => {
		if (xhr.status === 200) {
			const html = JSON.parse(xhr.responseText).result;
			const article = document.querySelector('form + article');
			article && article.remove();
			form.insertAdjacentHTML('afterend', html);
			Array.prototype.forEach.call(document.querySelectorAll('pre code'), hljs.highlightBlock);
		} else {
			console.log(`/blog/preview request failed; returned status ${xhr.status}.`);
		}
	};
	xhr.send(data);
});

saveBtn.addEventListener('click', () => {
	const data = new FormData(form);
	const xhr = new XMLHttpRequest();
	xhr.open('POST', '/blog/save');
	xhr.onload = () => {
		if (xhr.status === 200) {
			const obj = JSON.parse(xhr.responseText).result;
			window.location = obj.redirectUrl;
		} else {
			console.log(`/blog/save request failed; returned status ${xhr.status}.`);
		}
	};
	xhr.send(data);
});