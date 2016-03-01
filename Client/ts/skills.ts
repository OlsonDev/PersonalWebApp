const filter = document.getElementById('skills-filter');
const search = filter.querySelector('input[type=search]') as HTMLInputElement;
const skills = document.getElementById('skills');
const filterFormToggle = document.getElementById('filter-form-toggle');
const filterTags = document.getElementById('filter-tags');
const clearFilters = document.getElementById('clear-filters');

let collapsed = true;

filterFormToggle.addEventListener('click', () => {
	collapsed = !collapsed;
	filter.classList.toggle('collapsed', collapsed);
	filter.classList.toggle('expanded', !collapsed);
	if (collapsed) return;
	search.focus();
});

let tags: string[] = [];

// Delegate click handler instead of listening on every element
filterTags.addEventListener('click', (e) => {
	if (!(e.target instanceof HTMLLIElement)) return;
	const target = e.target as HTMLLIElement;
	target.classList.toggle('active');
	const tagResult = /(?:^| )(tag-\S+)/.exec(target.className);
	const tag = tagResult.length ? tagResult[1] : '';

	if (tags.indexOf(tag) === -1) {
		// Add to filter
		const elems = skills.querySelectorAll(`:not(.${tag})`);
		for (let i = 0; i < elems.length; i++) {
			elems[i].classList.add('filtered');
		}
	} else {
		// Remove from filter
		console.log('TODO: Remove from filter');
	}
});

clearFilters.addEventListener('click', () => {
	search.value = '';
	tags = [];
	let elems = skills.children;
	for (let i = 0; i < elems.length; i++) {
		elems[i].classList.remove('filtered');
	}
	elems = filterTags.children;
	for (let i = 0; i < elems.length; i++) {
		elems[i].classList.remove('active');
	}
	search.focus();
});