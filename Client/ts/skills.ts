'use strict';
(() => {
	let searchValue = '';
	let tagValue = '';
	let modal: JQuery;
	let skills: JQuery;
	let searchInput: JQuery;
	const oldOpenModal = $.fn.openModal;

	$.fn.openModal = function () {
		oldOpenModal.apply(this, arguments);
		// Undo body styles applied by oldOpenModal for anti-scroll behavior
		$(document.body).css({ overflow: '', width: '' });
	};

	function hideModal() {
		modal.slideUp(250, function () {
			$(this).closeModal();
		});
	}

	function hideModalOnBodyClick(e: JQueryEventObject) {
		if (modal.has(e.target).length || modal.is(e.target)) {
			$(document.body).one('click', hideModalOnBodyClick);
			return;
		}
		hideModal();
	}

	function search() {
		skills.children().each(function (i, el) {
			const $skill = $(el);
			const name = $skill.find('.card-title').text().toLowerCase();
			let show = !tagValue || $skill.hasClass(tagValue);
			show = show && (!searchValue || name.indexOf(searchValue) !== -1);
			$skill.toggle(show);
		});
	}

	$(() => {
		modal = $('#tag-filter-modal');
		skills = $('#skills');
		searchInput = $('#search');

		searchInput.focus(function (e) {
			searchInput.siblings('.chip').addClass('active');
		});

		searchInput.blur(function (e) {
			searchInput.siblings('.chip').removeClass('active');
		});

		modal.on('click', '.chip', function (e) {
			const chip = $(this);

			if (chip.hasClass('active')) {
				chip.removeClass('active');
				tagValue = '';
			} else {
				const clazz = chip.prop('class');
				const result = clazz.match(/\btag-.+?\b/);
				const clone = chip.clone();

				chip.addClass('active');
				chip.siblings('.active').removeClass('active');
				tagValue = result.length ? result[0] : tagValue;
				
				const close = $('<i class="material-icons">close</i>');
				close.on('click', function () {
					tagValue = '';
					chip.removeClass('active');
					search();
				});
				clone.append(close);
				searchInput.siblings('.chip').remove();
				clone.insertBefore(searchInput);
			}

			search();
			setTimeout(() => {
				hideModal();
				$(document.body).off('click', hideModalOnBodyClick);
			}, 300);
		});

		$('#tag-filter-btn').click(function () {
			if (modal.hasClass('open')) return;
			modal.openModal();
			$('.modal.open').hide().slideDown(250);
			$('.lean-overlay').remove();
			setTimeout(() => $(document.body).one('click', hideModalOnBodyClick), 0);
		});

		searchInput.on('input', function() {
			searchValue = this.value.trim().toLowerCase();
			search();
		});
	});
}());