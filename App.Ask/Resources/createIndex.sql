create view post_view as
select p.*,
        p2.nick_name  as person_nick_name,
        p2.is_delete  as person_is_delete,
        p2.is_mute    as person_is_mute,
        p2.avatar     as person_avatar,
        t.name        as topic_name,
        t.is_hide     as topic_is_hide,
        t.is_announce as topic_is_announce
from post as p
            inner join person p2 on p2.id = p.person_id
            inner join topic t on p.topic_id = t.id;

create view person_view as
select p.*,
        pd.score,
        pd.article_num,
        pd.ask_num,
        pd.answer_num,
        pd.like_num,
        r.name      as role_name,
        r.role_type as role_type
from person p
            inner join role r on p.role_id = r.id
            inner join person_data pd on p.id = pd.person_id;

create view comment_view as select comment.*,p.nick_name,p.avatar from comment inner join person p on comment.person_id = p.id;

create view activity_view as
select a.*,
        person.account_name as person_account_name,
        person.nick_name as person_nick_name,
        person.avatar as person_avatar,
        person.is_delete as person_is_delete,
        person.is_mute as person_is_mute,
        post.title as post_title,
        post.post_type,
        post.post_status,
        post.topic_id,
        t.name as topic_name,
        t.is_hide as topic_is_hide,
        t.is_announce as topic_is_announce
from activity as a
            inner join person on person.id = a.person_id
            inner join post on post.id = a.post_id
            inner join topic t on post.topic_id = t.id;