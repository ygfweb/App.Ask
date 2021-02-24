--创建表
CREATE TABLE config(
id uuid PRIMARY KEY,
config_key varchar(500) not null ,
config_value varchar(500) not null 
);



CREATE TABLE comment(
id uuid PRIMARY KEY,
parent_id uuid ,
post_id uuid not null ,
person_id uuid not null ,
html_content text not null ,
text_content text not null ,
create_time TIMESTAMP not null ,
modify_time TIMESTAMP ,
is_delete boolean not null ,
like_num int not null 
);

CREATE INDEX idx_comment_post_id on comment(post_id);
CREATE INDEX idx_comment_person_id on comment(person_id);
CREATE INDEX idx_comment_create_time on comment(create_time);


CREATE TABLE honor(
id uuid PRIMARY KEY,
name varchar(500) not null 
);



CREATE TABLE invite_code(
id uuid PRIMARY KEY,
person_id uuid not null ,
code varchar(500) not null 
);



CREATE TABLE person(
id uuid PRIMARY KEY,
account_name varchar(500) not null ,
nick_name varchar(500) not null ,
password varchar(500) not null ,
role_id uuid not null ,
create_time TIMESTAMP not null ,
last_updated TIMESTAMP not null ,
introduction varchar(500) not null ,
signature varchar(500) not null ,
avatar varchar(500) not null ,
is_delete boolean not null ,
is_mute boolean not null 
);

CREATE INDEX idx_person_account_name on person(account_name);
CREATE INDEX idx_person_nick_name on person(nick_name);
CREATE INDEX idx_person_role_id on person(role_id);
CREATE INDEX idx_person_is_delete on person(is_delete);
CREATE INDEX idx_person_is_mute on person(is_mute);


CREATE TABLE person_data(
id uuid PRIMARY KEY,
person_id uuid not null ,
score int not null ,
article_num int not null ,
ask_num int not null ,
answer_num int not null ,
like_num int not null 
);

CREATE INDEX idx_person_data_person_id on person_data(person_id);
CREATE INDEX idx_person_data_answer_num on person_data(answer_num);
CREATE INDEX idx_person_data_like_num on person_data(like_num);


CREATE TABLE post(
id uuid PRIMARY KEY,
title varchar(500) not null ,
create_time TIMESTAMP not null ,
modify_time TIMESTAMP ,
person_id uuid not null ,
topic_id uuid not null ,
post_type smallint not null ,
is_top boolean not null ,
is_best boolean not null ,
post_status smallint not null ,
comment_num int not null ,
like_num int not null ,
collect_num int not null ,
view_num int not null 
);

CREATE INDEX idx_post_create_time on post(create_time);
CREATE INDEX idx_post_person_id on post(person_id);
CREATE INDEX idx_post_topic_id on post(topic_id);
CREATE INDEX idx_post_post_type on post(post_type);
CREATE INDEX idx_post_is_top on post(is_top);
CREATE INDEX idx_post_is_best on post(is_best);
CREATE INDEX idx_post_post_status on post(post_status);
CREATE INDEX idx_post_comment_num on post(comment_num);
CREATE INDEX idx_post_like_num on post(like_num);
CREATE INDEX idx_post_collect_num on post(collect_num);
CREATE INDEX idx_post_view_num on post(view_num);


CREATE TABLE post_data(
id uuid PRIMARY KEY,
post_id uuid not null ,
html_content text not null ,
text_content text not null 
);

CREATE INDEX idx_post_data_post_id on post_data(post_id);


CREATE TABLE post_tag(
id uuid PRIMARY KEY,
post_id uuid not null ,
tag_id uuid not null 
);

CREATE INDEX idx_post_tag_post_id on post_tag(post_id);
CREATE INDEX idx_post_tag_tag_id on post_tag(tag_id);


CREATE TABLE role(
id uuid PRIMARY KEY,
name varchar(500) not null ,
role_type smallint not null 
);

CREATE INDEX idx_role_name on role(name);
CREATE INDEX idx_role_role_type on role(role_type);


CREATE TABLE tag(
id uuid PRIMARY KEY,
name varchar(500) not null ,
is_best boolean not null ,
is_system boolean not null 
);

CREATE INDEX idx_tag_name on tag(name);
CREATE INDEX idx_tag_is_best on tag(is_best);
CREATE INDEX idx_tag_is_system on tag(is_system);


CREATE TABLE topic(
id uuid PRIMARY KEY,
name varchar(500) not null ,
is_hide boolean not null ,
is_announce boolean not null ,
order_num int not null 
);

CREATE INDEX idx_topic_name on topic(name);
CREATE INDEX idx_topic_is_hide on topic(is_hide);


CREATE TABLE user_honor(
id uuid PRIMARY KEY,
user_id uuid not null ,
honor_id uuid not null 
);

CREATE INDEX idx_user_honor_user_id on user_honor(user_id);
CREATE INDEX idx_user_honor_honor_id on user_honor(honor_id);


CREATE TABLE activity(
id uuid PRIMARY KEY,
person_id uuid not null ,
post_id uuid not null ,
activity_type smallint not null ,
do_time TIMESTAMP not null 
);

CREATE INDEX idx_activity_person_id on activity(person_id);
CREATE INDEX idx_activity_post_id on activity(post_id);
CREATE INDEX idx_activity_activity_type on activity(activity_type);
CREATE INDEX idx_activity_do_time on activity(do_time);


CREATE TABLE favorite(
id uuid PRIMARY KEY,
person_id uuid not null ,
post_id uuid not null ,
do_time TIMESTAMP not null 
);

CREATE INDEX idx_favorite_person_id on favorite(person_id);
CREATE INDEX idx_favorite_post_id on favorite(post_id);
CREATE INDEX idx_favorite_do_time on favorite(do_time);


CREATE TABLE zan(
id uuid PRIMARY KEY,
zan_type smallint not null ,
session_id varchar(500) not null ,
person_id uuid ,
post_id uuid ,
comment_id uuid ,
do_time TIMESTAMP not null 
);

CREATE INDEX idx_zan_session_id on zan(session_id);
CREATE INDEX idx_zan_person_id on zan(person_id);
CREATE INDEX idx_zan_post_id on zan(post_id);
CREATE INDEX idx_zan_comment_id on zan(comment_id);
CREATE INDEX idx_zan_do_time on zan(do_time);


CREATE TABLE follow(
id uuid PRIMARY KEY,
from_person_id uuid not null ,
to_person_id uuid not null ,
do_time TIMESTAMP not null 
);

CREATE INDEX idx_follow_from_person_id on follow(from_person_id);
CREATE INDEX idx_follow_to_person_id on follow(to_person_id);