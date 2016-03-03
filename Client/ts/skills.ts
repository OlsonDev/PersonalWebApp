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

function updateFilter(liElems: NodeListOf<Element>) {
	const searchValue = search.value.trim().toLowerCase();
	for (let i = 0; i < liElems.length; i++) {
		const liElem = liElems[i] as HTMLLIElement;
		liElem.classList.toggle('filtered', shouldBeFiltered(liElem, searchValue));
	}
}

function shouldBeFiltered(liElem: HTMLLIElement, searchValue: string) {
	for (const tag of tags) {
		if (liElem.classList.contains(tag)) continue;
		return true;
	}
	if (searchValue === '') return false;
	const name = liElem.querySelector('h3').textContent.toLowerCase();
	return name.indexOf(searchValue) === -1;
}

// Delegate click handler instead of listening on every element
filterTags.addEventListener('click', (e) => {
	if (!(e.target instanceof HTMLLIElement)) return;
	const target = e.target as HTMLLIElement;
	target.classList.toggle('active');
	const tagResult = /(?:^| )(tag-\S+)/.exec(target.className);
	const tag = tagResult.length ? tagResult[1] : '';
	if (!tag) return;

	const elems = skills.querySelectorAll(`:not(.${tag})`);
	if (tags.indexOf(tag) === -1) {
		tags.push(tag);
		for (let i = 0; i < elems.length; i++) {
			elems[i].classList.add('filtered');
		}
	} else {
		const index = tags.indexOf(tag);
		tags.splice(index, 1);
		updateFilter(elems);
	}
});

search.addEventListener('input', () => updateFilter(skills.children));

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