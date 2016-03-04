function renderCaptcha() {
	grecaptcha.render('grecaptcha', {
		sitekey: '6LeIihgTAAAAAGJIaWSkdtXxZYiGY4y5ziVDOBbY'
		, callback: (response: string) => {
			const xhr = new XMLHttpRequest();
			xhr.open('post', '/contact/info');
			xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
			xhr.onload = () => {
				if (xhr.status === 200) {
					const obj = JSON.parse(xhr.responseText);
					document.getElementById('email').textContent = obj.email;
					document.getElementById('phone').textContent = obj.phone;
				} else {
					console.log(`Request failed; returned status ${xhr.status}.`);
				}
			};
			xhr.send(encodeURI(`gRecaptchaResponse=${response}`));
		}
	});
}