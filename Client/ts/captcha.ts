function renderCaptcha() {
	grecaptcha.render('grecaptcha', {
		sitekey: '6LeIihgTAAAAAGJIaWSkdtXxZYiGY4y5ziVDOBbY'
		, callback: (response: string) => {
			const xhr = new XMLHttpRequest();
			xhr.open('post', '/contact/info');
			xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
			xhr.onload = () => {
				if (xhr.status === 200) {
					const obj = JSON.parse(xhr.responseText).result;
					const email = document.getElementById('email') as HTMLAnchorElement;
					email.href = `mailto:${obj.email}?subject=Hello from your website&body=Hey there,%0D%0A%0D%0A`;
					email.textContent = obj.email;
					document.getElementById('phone').textContent = obj.phone;
					document.getElementById('contact-info').classList.remove('hidden');
					document.getElementById('grecaptcha').classList.add('hidden');
				} else {
					console.log(`Request failed; returned status ${xhr.status}.`);
				}
			};
			xhr.send(encodeURI(`gRecaptchaResponse=${response}`));
		}
	});
}